using System.Collections.Generic;

namespace BuildScreen.Dto.Octopus
{
    public class Dashboard
    {
        public List<Project> Projects { get; set; }
        public List<ProjectGroup> ProjectGroups { get; set; }
        public List<Environment> Environments { get; set; }
        public List<Item> Items { get; set; }
        public List<Item> PreviousItems { get; set; }
        public Links Links { get; set; }
    }

    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ProjectGroupId { get; set; }
        public Links Links { get; set; }
    }

    public class ProjectGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> EnvironmentIds { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string Self { get; set; }
        public string Release { get; set; }
        public string Task { get; set; }
    }

    public class Environment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Links Links { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string EnvironmentId { get; set; }
        public string ReleaseId { get; set; }
        public string DeploymentId { get; set; }
        public string TaskId { get; set; }
        public string ReleaseVersion { get; set; }
        public string Created { get; set; }
        public string QueueTime { get; set; }
        public string CompletedTime { get; set; }
        public string State { get; set; }
        public bool HasPendingInterruptions { get; set; }
        public bool HasWarningsOrErrors { get; set; }
        public string ErrorMessage { get; set; }
        public string Duration { get; set; }
        public Links Links { get; set; }
    }
}