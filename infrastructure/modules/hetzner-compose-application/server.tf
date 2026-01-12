resource "hcloud_server" "server" {
  # Admin configuration
  ssh_keys = [hcloud_ssh_key.admin.id]

  # Server configuration
  name = "${var.application_name}-server"
  image = "debian-13"
  server_type = var.server_type
  location = var.server_location
  
  # Runtime configuration
  user_data = base64encode(local.cloud_init)
}
