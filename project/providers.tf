terraform {
  required_providers {
    github = {
      source = "integrations/github"
      version = "6.9.1"
    }
  }
}

provider "github" {
  token = var.github_token
}

variable "github_token" {
  description = "GitHub token for project remote repository administration"
  type = string
  sensitive = true
}
