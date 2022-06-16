terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.10.0"
    }
  }
}

provider "azurerm" {
  features{}
}

//Resource Group
resource "azurerm_resource_group" "rg" {
  name = "TP1"
  location = "southcentralus"
}

resource "azurerm_resource_group" "rgvm" {
  name = "TP1_VM"
  location = "southcentralus"
}

//Virtual Network
resource "azurerm_virtual_network" "vn" {
  name                = "tp1-network"
  location            = azurerm_resource_group.rgvm.location
  resource_group_name = azurerm_resource_group.rgvm.name
  address_space       = ["192.168.0.0/22"]

  subnet {
    name           = "TP1_SubNetwork"
    address_prefix = "192.168.0.0/22"
  }
}

//MSSQL SERVER
resource "azurerm_mssql_server" "mssql_serv" {
  name                         = "mssql-sqlserver"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "logintest" //A changer
  administrator_login_password = "mdptest1234" //A changer
}

//MSSQL DataBase
resource "azurerm_mssql_database" "mssql_db" {
  name           = "mssql_db"
  server_id      = azurerm_mssql_server.mssql_serv.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 10
  read_scale     = false
  sku_name       = "S0"
  zone_redundant = false
}

//Service Plan
resource "azurerm_app_service_plan" "asp" {
  name                = "api-appserviceplan-pro"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  sku {
    tier = "Free"
    size = "F1"
  }
}

//Web App
resource "azurerm_windows_web_app" "wwa" {
  name                = "AzureGamingDeGregoire"  //A changer
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id = azurerm_app_service_plan.asp.id

  site_config {
    application_stack {
      current_stack = "dotnet"
      dotnet_version = "v6.0"
    }
    always_on         = false // Required for F1 plan (even though docs say that it defaults to false)
    use_32_bit_worker = true // Required for F1 plan
  }
}