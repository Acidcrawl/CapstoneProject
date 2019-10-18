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
            CompletedDate = null;
            StartedDate = null;
        }

        public Task(string newName, int newStart, int newDuration) {
            this.Name = newName;
            this.Start = newStart;
            this.MinDuration = newDuration;
            this.DependentTasks = new List<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float MinDuration { get; set; }
        public float MaxDuration { get; set; }
        public float MostLikelyDuration { get; set; }
        public int Priority { get; set; }
        public Nullable<DateTime> CompletedDate { get; set; }
        public Nullable<DateTime> StartedDate { get; set; }
        public int Start { get; set; } // TODO: Use actual dateTime for calculations.
        public DateTime DeletedDate { get; set; }

        public void AddDependentTask(Task t) {
            this.DependentTasks.Add(t);
        }

        //modified date created by alankar pokhrel
        public DateTime ModifiedDate { get; set; }

        public List<Task> DependentTasks { get; set; }
        public User Owner { get; set; }
        public Status Status { get; set; }
        public Project Project { get; set; }
    }
}

