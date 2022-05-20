using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Monitor.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.IO;


namespace Monitor.Helpers 
{

    public  class Requesters 
    {

        //Confere se há algum site e salva as informações em response. Caso contrário houve algum erro na requisição.
        public static async Task<HttpStatusCode> GetStatusFromUrl(string url) 
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                return response.StatusCode;
            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.NotFound;
            }
        }
    }
}