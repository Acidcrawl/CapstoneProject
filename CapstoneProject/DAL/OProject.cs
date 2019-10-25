/*
 * Created by Alankar Pokhrel 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CapstoneProject.Models;


namespace CapstoneProject.DAL
{
    public class OProject
    {
        SqlConnection conn;


        public OProject()
        {
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\SmartPertDB.mdf;Integrated Security=True");
        }

        public Project Insert(Project newProject)
        {
            conn.Open();
            string query = "insert into Project(Name, StartDate, WorkingHours, ProjectOwner) output INSERTED.ProjectId values(@name, @startDate, @workingHours, @projectOwner)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", newProject.Name);
            cmd.Parameters.AddWithValue("@startDate",((object)newProject.StartDate) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@workingHours",newProject.WorkingHours);
            cmd.Parameters.AddWithValue("@projectOwner",newProject.ProjectOwner.Id);
            int effectedIds = (int) cmd.ExecuteScalar();
            newProject.Id = effectedIds;
            conn.Close();
            return newProject;
        }
        public int Delete(int projectId)
        {
            conn.Open();
            string query = "Delete from Project Where ProjectId= " + projectId;
            SqlCommand cmd = new SqlCommand(query, conn);
            int effectedIds = cmd.ExecuteNonQuery();
            conn.Close();
            return effectedIds;
        }
        public int Update(Project updatedProject)
        {
            conn.Open();
            string query = "update Project set Name = @name, WorkingHours=@workingHours, ProjectOwner=@projectOwner Where ProjectId=@projectid";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", updatedProject.Name);
            cmd.Parameters.AddWithValue("@workingHours", updatedProject.WorkingHours);
            cmd.Parameters.AddWithValue("@projectOwner", updatedProject.ProjectOwner);
            cmd.Parameters.AddWithValue("@projectid", updatedProject.Id);
            int effectedIds = cmd.ExecuteNonQuery();
            conn.Close();
            return effectedIds;

        }
        public List<Project> Select()
        {
            conn.Open();
            string query = "Select * from Project";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Project> projectList = new List<Project>();
            while (reader.Read())
            {
                Project project = new Project();
                project.Id = (int)reader["ProjectId"];
                project.Name = (string)reader["Name"];
                OUser oUser = new OUser();
                project.ProjectOwner = oUser.SelectSingleUser((int)reader["ProjectOwner"]);
                project.StartDate = (DateTime)reader["StartDate"];
                project.WorkingHours = (double)reader["WorkingHours"];
                if ((reader["Description"]) != DBNull.Value)
                    project.Description = (string)reader["Description"];
                projectList.Add(project);
            }
            return projectList;
        }
    
    }
}
