# Admin configuration
variable "admin_ssh_key" {
  description = "SSH public key of the admin. A dummy key is used if not provided. And the server is created without SSH access."
  type = string
  default = "ssh-ed25519 0000000000000000000000000000000000000000000000000000000000000000"
}

# Server configuration
variable "server_type" {
  description = "Type of the server"
  type = string
  default = "cpx11"
}

variable "server_location" {
  description = "Location of the server"
  type = string
  default = "nbg1"
}

# Application configuration

variable "application_name" {
  description = "Internal name of the application. Used for the server name."
  type = string
}

variable "git_repository_url" {
  description = "URL of the git repository. Only public repositories are supported."
  type = string
}

variable "git_repository_branch" {
  description = "Branch of the git repository"
  type = string
}

variable "compose_file_path" {
  description = "Path of the compose file. From the root of the git repository."
  type = string
}

variable "reconciliation_interval" {
  description = "Interval of the reconciliation. The reconciliation is the process of pulling the latest changes from the git repository and applying them to the server."
  type = string
}
