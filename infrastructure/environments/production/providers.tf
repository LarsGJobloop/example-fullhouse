terraform {
  required_providers {
    hcloud = {
      source = "hetznercloud/hcloud"
      version = "1.58.0"
    }
  }
}

provider "hcloud" {
  token = var.hcloud_token
}

variable "hcloud_token" {
  description = "Hetzner Cloud token"
  type = string
  sensitive = true
}
