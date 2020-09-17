using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.DTOs
{
    [NotMapped]
    public class JobDTO
    {
        public int jobId { get; set; }
        public int posterId { get; set; }
        public int boardId { get; set; }
        public string poster { get; set; }
        public bool isTaken { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string location { get; set; }
        public string title { get; set; }
        
        //we don't need to return the boardId here, this method is used to convert
        //jobs for a single board
        public static List<JobDTO> convertJobs(List<Job> jobs)
        {
            List<JobDTO> output = new List<JobDTO>();
            foreach(Job job in jobs)
            {
                JobDTO newJob = new JobDTO();
                newJob.jobId = job.jobId;
                newJob.posterId = job.posterId;
                newJob.description = job.jobDescription;
                newJob.isTaken = job.isTaken;
                newJob.location = job.location;
                newJob.poster = job.poster.user.UserName;
                newJob.thumbnail = job.mediaURI;
                newJob.title = job.jobTitle;

                output.Add(newJob);
            }

            return output;
        }
    }
}