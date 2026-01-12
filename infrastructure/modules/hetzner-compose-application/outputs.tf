output "application_address" {
  description = "Public IP address of the server"
  value = {
    ipv4 = hcloud_server.server.ipv4_address
    ipv6 = hcloud_server.server.ipv6_address
  }
  sensitive = false
}

output "cloud_init_rendered" {
  description = "Rendered cloud-init configuration file"
  value = local.cloud_init
  // WARNING! There might be sensitive information in the rendered cloud-init configuration file.
  // Ex:
  // - Repository access credentials
  // - SSH keys
  sensitive = false
}
