using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class Vehicles
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public String Make { get; set; }
        public String Model { get; set; }
    }

    public class Logger
    {
        public static void WriteToLogFile(string Message)
        {
            try
            {
                Message = DateTime.Now + " : " + Message;

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ApplicationLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";

                if (!File.Exists(filepath))
                {
                    using (Stream strmFile = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter LogStream = new StreamWriter(strmFile))
                    {
                        LogStream.WriteLine(Message);
                        LogStream.Dispose();

                    }
                }
                else
                {
                    using (Stream strmFile = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter LogStream = new StreamWriter(strmFile))
                    {
                        LogStream.WriteLine(Message);
                        LogStream.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Failed. " + exception.Message, EventLogEntryType.Error);
                }
            }
        }
    }

    public class BALDAL
    {
        string ConnectionString = @"data source=VM1122;initial catalog=master;user id=sa;password=myadmin";

        public List<Vehicles> DALGetVehicles()
        {

            DataTable VehicleSet = new DataTable();
            List<Vehicles> vehicleList = new List<Vehicles>();
            try
            {


                // string ConnectionString = @"data source=VM1122;initial catalog=master;trusted_connection=true";

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.Get_VehiclesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    VehicleSet = ds.Tables[0];
                    Logger.WriteToLogFile(VehicleSet.ToString());
                    con.Close();
                    foreach (DataRow vehicleDr in VehicleSet.Rows)
                    {
                        Vehicles item = new Vehicles();
                        item.Id = int.Parse(vehicleDr[0].ToString());
                        item.Year = int.Parse(vehicleDr[1].ToString());
                        item.Make = vehicleDr[2].ToString();
                        item.Model = vehicleDr[3].ToString();
                        vehicleList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile("DALGetVehicles :" + ex.Message);

            }
            return vehicleList;
        }

        public List<Vehicles> DALGetVehicleById(string id)
        {
            DataTable VehicleSet = new DataTable();
            List<Vehicles> vehicleList = new List<Vehicles>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.Get_VehicleDetailsByID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Int32.Parse(id.ToString());
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    VehicleSet = ds.Tables[0];
                    con.Close();
                    foreach (DataRow vehicleDr in VehicleSet.Rows)
                    {
                        Vehicles item = new Vehicles();
                        item.Id = int.Parse(vehicleDr[0].ToString());
                        item.Year = int.Parse(vehicleDr[1].ToString());
                        item.Make = vehicleDr[2].ToString();
                        item.Model = vehicleDr[3].ToString();
                        vehicleList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile("DALGetVehicleById :" + ex.Message);

            }
            return vehicleList;
        }

        public DataTable DALCreateVehicle(int year, string make, string model)
        {
            DataTable NewVehicle = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.Create_NewVehicle";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = int.Parse(year.ToString()); ;
                    cmd.Parameters.Add("@Make", SqlDbType.VarChar).Value = make.ToString();
                    cmd.Parameters.Add("@Model", SqlDbType.VarChar).Value = model.ToString();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    NewVehicle = ds.Tables[0];
                    con.Close();
                }
            }
            catch (Exception exception)
            {
                Logger.WriteToLogFile("DALCreateVehicle :" + exception.Message);
            }
            return NewVehicle;
        }

        public DataTable DALUpdateVehicle(int id, int year, string make, string model)
        {
            DataTable UpdateVehicle = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "[dbo].[Update_VehicleDetails]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(id.ToString());
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = int.Parse(year.ToString()); ;
                    cmd.Parameters.Add("@Make", SqlDbType.VarChar).Value = make.ToString();
                    cmd.Parameters.Add("@Model", SqlDbType.VarChar).Value = model.ToString();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    UpdateVehicle = ds.Tables[0];
                    con.Close();
                }
            }
            catch (Exception exception)
            {
                Logger.WriteToLogFile("DALUpdateVehicle :" + exception.Message);
            }
            return UpdateVehicle;
        }

        public DataTable DALDeleteVehicle(string id)
        {
            DataTable DeleteVehicle = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "[dbo].[Remove_VehicleDetails]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(id.ToString());
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    DeleteVehicle = ds.Tables[0];
                    con.Close();
                }
            }
            catch (Exception exception)
            {
                Logger.WriteToLogFile("DALDeleteVehicle :" + exception.Message);
            }
            return DeleteVehicle;
        }
    }
}