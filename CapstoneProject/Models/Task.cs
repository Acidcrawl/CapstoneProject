/*
 * Created by Levi Delezene 
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//added namespace-alankar
namespace CapstoneProject.Models
{
    public enum Status
    {
        notStarted, inProgress, completed
    }

    public class Task
    {
        //These classes are using auto properties. I don't know how they'll work with the database stuff.
        //It's easy enough to change if they won't work

        public Task()
        {
            this.DependentTasks = new List<Task>();
        }

        public Task(string newName, DateTime newStart, int newDuration, Boolean newRootNode) {
            this.Name = newName;
            this.StartedDate = newStart;
            this.MinDuration = newDuration;
            this.DependentTasks = new List<Task>();
            this.RootNode = newRootNode;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MinDuration { get; set; }
        public double MaxDuration { get; set; }
        public double MostLikelyDuration { get; set; }
        public int Priority { get; set; }
        public Nullable<DateTime> CompletedDate { get; set; }
        public Nullable<DateTime> StartedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public Boolean RootNode { get; set; }

        public void AddDependentTask(Task t) {
            this.DependentTasks.Add(t);
        }

        //modified date created by alankar pokhrel
        public Nullable<DateTime> ModifiedDate { get; set; }

        public List<Task> DependentTasks { get; set; }
        public User Owner { get; set; }
        public Status Status { get; set; }
        public int ProjectId { get; set; }
    }
}

