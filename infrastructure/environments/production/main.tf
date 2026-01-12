module "compose_server" {
  source = "../../modules/hetzner-compose-application"

  # Admin configuration
  admin_ssh_key = file("admin-key.pub")

  # Server configuration
  server_type = "cax31" # 8vCPU, 16GB RAM, 160GB SSD
  server_location = "hel1" # Helsinki, Finland

  # Application configuration
  application_name = "fullhouse"
  git_repository_url = "https://github.com/LarsGJobloop/example-fullhouse.git"
  git_repository_branch = "main"
  compose_file_path = "compose.yaml"
  reconciliation_interval = "30s"
}

output "compose_server" {
  description = "Compose Application Server"
  value = module.compose_server
  sensitive = false
}
