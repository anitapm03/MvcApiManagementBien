using MvcApiManagementBien.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MvcApiManagementBien.Services
{
    public class ServiceApiManagement
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiEmpleados;
        private string UrlApiDepartamentos;

        public ServiceApiManagement(IConfiguration configuration)
        {
            this.Header = new MediaTypeWithQualityHeaderValue
                ("application/json");
            this.UrlApiEmpleados = configuration.GetValue<string>
                ("ApiUrls:ApiEmpleados");
            this.UrlApiDepartamentos =
                configuration.GetValue<string>
                ("ApiUrls:ApiDepartamentos");
        }


        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //debemos enviar una cadena vacia 
                //al final del request
                var queryString =
                    HttpUtility.ParseQueryString(string.Empty);

                string request = "data?" + queryString;
                //no se utiliza baseaddress
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(this.Header);
                httpClient.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");

                //la peticion se realiza en conjunto
                //url+request
                HttpResponseMessage responseMessage = await
                    httpClient.GetAsync(this.UrlApiEmpleados + request);

                if (responseMessage.IsSuccessStatusCode)
                {
                    List<Empleado> empleados = await
                        responseMessage.Content.ReadAsAsync<List<Empleado>>();
                    return empleados;
                }
                else
                {
                    return null;
                }
            }
        }

        //metodo con subscripción
        public async Task<List<Departamento>> GetDepartamentosAsync(string suscripcion)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //debemos enviar una cadena vacia 
                //al final del request
                var queryString =
                    HttpUtility.ParseQueryString(string.Empty);

                string request = "api/departamentos?" + queryString;
                //no se utiliza baseaddress
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(this.Header);
                httpClient.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");

                //debemos añadir la subscripción mediante una key
                httpClient.DefaultRequestHeaders.Add
                    ("Ocp-Apim-Subscription-Key", suscripcion);

                //la peticion se realiza en conjunto
                //url+request
                HttpResponseMessage responseMessage = await
                    httpClient.GetAsync(this.UrlApiDepartamentos + request);

                if (responseMessage.IsSuccessStatusCode)
                {
                    List<Departamento> departamentos = await
                        responseMessage.Content.ReadAsAsync<List<Departamento>>();
                    return departamentos;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
