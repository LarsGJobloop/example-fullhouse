resource "hcloud_ssh_key" "admin" {
  name = "${var.application_name}-admin"
  public_key = var.admin_ssh_key
}
