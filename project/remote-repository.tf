resource "github_repository" "remote_repository" {
  name = "example-fullhouse"
  description = "Reference implementation of a strictly governed C# API"

  visibility = "public"
}

output "remote_repository_url" {
  description = "URL of the remote repository"
  value = github_repository.remote_repository.http_clone_url
  sensitive = false
}