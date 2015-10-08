using ModernHttpClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.ES
{
    public class BicimadContract : Contract
    {
        public BicimadContract()
        {
            ServiceProvider = "BiciMAD";
            StationsUrl = "http://www.infobicimad.com/estaciones.json.cript";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var clearResponse = Decrypt(responseBodyAsText);
                var pattern = @"({.*})";
                if (Regex.IsMatch(clearResponse, pattern))
                {
                    var regex = new Regex(pattern).Match(clearResponse);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        return JsonConvert.DeserializeObject<BicimadModel>(regex.Captures[0].Value).Stations.ToList<StationModelBase>();
                    }
                }
            }
            return null;
        }

        private static string Decrypt(string cryptedMessage)
        {
            byte[] message = Convert.FromBase64String(cryptedMessage);

            // BouncyCastle version
            var Hmac = new Org.BouncyCastle.Crypto.Digests.Sha512Digest();
            var keyToHash = Encoding.UTF8.GetBytes("Vg*b51^_! j4a\"I-d73j");
            Hmac.BlockUpdate(keyToHash, 0, keyToHash.Length);
            byte[] result = new byte[Hmac.GetDigestSize()];
            Hmac.DoFinal(result, 0);
            var array = new byte[24];
            Array.Copy(result, array, array.Length);
            var DesEngine = new PaddedBufferedBlockCipher(new DesEdeEngine());
            DesEngine.Init(false, new KeyParameter(array));
            byte[] output = new byte[message.Length];
            DesEngine.DoFinal(message, output, 0);
            return Encoding.UTF8.GetString(output, 0, output.Length);

            // PCLCrypto version
            //var hashEngine = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha512);
            //var hash = hashEngine.HashData(Encoding.UTF8.GetBytes("Vg*b51^_! j4a\"I-d73j"));
            //var desEngine = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.TripleDesEcb);
            //var key = new byte[24];
            //Array.Copy(hash, key, key.Length);
            //var cryptoKey = desEngine.CreateSymmetricKey(key);
            //var bytes = WinRTCrypto.CryptographicEngine.Decrypt(cryptoKey, message);
            //return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
