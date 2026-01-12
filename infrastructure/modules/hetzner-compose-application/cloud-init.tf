locals {
  # Scripts
  initialize_compose_host_base64 = base64encode(templatefile("${path.module}/scripts/initialize-compose-host.sh", {
    git_repository_url = var.git_repository_url
    git_repository_branch = var.git_repository_branch
  }))

  reconcile_compose_application_base64 = base64encode(templatefile("${path.module}/scripts/reconcile-compose-deployment.sh", {
    git_repository_branch = var.git_repository_branch
    compose_file_path = var.compose_file_path
  }))
}

locals {
  # Cloud Init
  cloud_init = templatefile("${path.module}/cloud-init.tmpl.yaml", {    
    # Repo configuration
    git_repository_url = var.git_repository_url
    git_repository_branch = var.git_repository_branch
    # Compose application configuration
    compose_file_path = var.compose_file_path
    # Reconciliation configuration
    reconciliation_interval = var.reconciliation_interval
    # Scripts
    initialize_compose_host_base64 = local.initialize_compose_host_base64
    reconcile_compose_application_base64 = local.reconcile_compose_application_base64
  })
}
