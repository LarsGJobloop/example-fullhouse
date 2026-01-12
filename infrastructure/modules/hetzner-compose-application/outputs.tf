output "application_address" {
  description = "Public IP address of the server"
  value = {
    ipv4 = hcloud_server.server.ipv4_address
    ipv6 = hcloud_server.server.ipv6_address
  }
  sensitive = false
}

# WARNING! There might be sensitive information in the rendered cloud-init file or it's parts.
# Ex:
# - Repository access credentials
# - SSH keys
# - Environment variables
# - Secrets
# - etc.
# That's not the case, **currently**.
output "cloud_init_rendered" {
  description = "Rendered cloud-init configuration file"
  value = local.cloud_init
  sensitive = false
}

output "initilzation_script_rendered" {
  description = "Rendered initialization script"
  value = base64decode(local.initialize_compose_host_base64)
  sensitive = false
}

output "reconciliation_script_rendered" {
  description = "Rendered reconciliation script"
  value = base64decode(local.reconcile_compose_application_base64)
  sensitive = false
}
