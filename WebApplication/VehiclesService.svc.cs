using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebApplication
{
      public class VehiclesService : IVehiclesService
    {
        BALDAL obj = new BALDAL();

        List<Vehicles> IVehiclesService.GetVehicle()
        {
            List<Vehicles> VIteamList = obj.DALGetVehicles();
            return VIteamList;
        }

        /*provide details of all vehicles*/
       
      

        /*Validation for range of Year*/
        public Boolean yearRange(int year)
        {
            if (year >= 1950 && year <= 2050)
                return true;
            else
                return false;
        }

        /*Check for empty or null string*/
        public Boolean IsEmpty(String str)
        {
            return String.IsNullOrEmpty(str);
        }



       
        List<Vehicles> IVehiclesService.getVehicleById(string id)
        {
            List<Vehicles> VIteamList = obj.DALGetVehicleById(id);
            return VIteamList;
        }

        string IVehiclesService.CreateVehicle(Vehicle vehicle)
        {
            string message;

            DataTable dt = obj.DALCreateVehicle(vehicle.Year, vehicle.Make, vehicle.Model);
            message = dt.Rows[0]["Messages"].ToString();

            return message;
        }

        string IVehiclesService.UpdateVehicle(Vehicle vehicle)
        {
            string message;

            DataTable dt = obj.DALUpdateVehicle(vehicle.Id, vehicle.Year, vehicle.Make, vehicle.Model);
            message = dt.Rows[0]["Messages"].ToString();

            return message;
        }

        string IVehiclesService.DeleteVehicle(string id)
        {
            string message;

            DataTable dt = obj.DALDeleteVehicle(id);
            message = dt.Rows[0]["Messages"].ToString();

            return message;
        }


    }
}
