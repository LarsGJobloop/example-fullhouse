locals {
  cloud_init = templatefile("${path.module}/cloud-init.tmpl.yaml", {    
    # Repo configuration
    git_repository_url = var.git_repository_url
    git_repository_branch = var.git_repository_branch
    # Compose application configuration
    compose_file_path = var.compose_file_path
    # Reconciliation configuration
    reconciliation_interval = var.reconciliation_interval
  })
}
