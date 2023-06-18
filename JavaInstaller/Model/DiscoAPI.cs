using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JavaInstaller.Model
{
    public class DiscoAPI
    {
        private readonly HttpClient httpClient;

        public DiscoAPI()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<string>> GetMajorVersions()
        {
            List<string> majorVersions = new List<string>();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://api.foojay.io/disco/v3.0/major_versions?ea=false&ga=true&maintained=true&include_build=false&include_versions=false");
                response.EnsureSuccessStatusCode(); // Ensure successful response

                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                var result = data.result;

                foreach (var item in result)
                {
                    majorVersions.Add(item.major_version.ToString());
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            return majorVersions;
        }

        public async Task<Dictionary<string, string>> GetDistributionVersions(string majorVersion)
        {
            Dictionary<string, string> distributionVersions = new Dictionary<string, string>();

            try
            {
                string apiUrl = $"https://api.foojay.io/disco/v3.0/distributions/versions/{majorVersion}?include_versions=false&include_synonyms=false";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Ensure successful response

                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                var result = data.result;

                foreach (var item in result)
                {
                    string name = item.name.ToString();
                    string distribution = item.api_parameter.ToString();
                    distributionVersions[name] = distribution;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            return distributionVersions;
        }

        public async Task<Dictionary<string, string>> GetPackageVersionAndDownload(string jdkVersion, string distribution)
        {
            Dictionary<string, string> packageVersions = new Dictionary<string, string>();

            try
            {
                string apiUrl = $"https://api.foojay.io/disco/v3.0/packages/jdks?jdk_version={jdkVersion}&distribution={distribution}&architecture=x64&archive_type=zip&operating_system=windows&release_status=ga&latest=available";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Ensure successful response

                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                var result = data.result;

                foreach (var item in result)
                {
                    string distributionVersion = item.distribution_version.ToString();
                    string downloadRedirect = item.links.pkg_download_redirect.ToString();
                    packageVersions[distributionVersion] = downloadRedirect;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            return packageVersions;
        }
    }
}
