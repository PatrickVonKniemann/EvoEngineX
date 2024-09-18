provider "azurerm" {
  features {}
}

# Azure Resource Group
resource "azurerm_resource_group" "my_dotnet_rg" {
  name     = "my-dotnet-api-group"
  location = var.azure_location
}

# Azure Container Registry (ACR)
resource "azurerm_container_registry" "my_dotnet_acr" {
  name                = "myacrregistry"
  resource_group_name = azurerm_resource_group.my_dotnet_rg.name
  location            = azurerm_resource_group.my_dotnet_rg.location
  sku                 = "Basic"
  admin_enabled       = true
}

# Azure Container Instance (ACI)
resource "azurerm_container_group" "my_dotnet_aci" {
  name                = "my-dotnet-api-group"
  location            = azurerm_resource_group.my_dotnet_rg.location
  resource_group_name = azurerm_resource_group.my_dotnet_rg.name
  os_type             = "Linux"

  container {
    name   = "my-dotnet-api"
    image  = "${azurerm_container_registry.my_dotnet_acr.login_server}/my-dotnet-api:latest"
    cpu    = "0.5"
    memory = "1.5"

    ports {
      port     = 80
      protocol = "TCP"
    }
  }

  ip_address {
    type = "public"
    ports {
      port     = 80
      protocol = "TCP"
    }
  }
}
