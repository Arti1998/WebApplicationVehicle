using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVehiclesService" in both code and config file together.
    [ServiceContract]
    public interface IVehiclesService
    {
        [WebGet(UriTemplate = "/vehicles", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Vehicles> GetVehicle();

        [WebGet(UriTemplate = "/vehicles/{id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Vehicles> getVehicleById(String id);

        [WebInvoke(Method = "PUT", UriTemplate = "/updatevehicles", RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string UpdateVehicle(Vehicle vehicle);

        [WebInvoke(Method = "POST", UriTemplate = "/AddVehicles", RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string CreateVehicle(Vehicle vehicle);

        [WebInvoke(Method = "DELETE", UriTemplate = "/RemoveVehicles/{id}")]
        [OperationContract]
        string DeleteVehicle(string id);


    }

    public class Vehicle
    {
        //bool boolValue = true;
        //string stringValue = "Hello ";

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }

    }
}
