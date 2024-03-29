<#                                                                                               
 Program Name : ServerDiscovery.ps1
                                                                                              
 Original Author: Hamid Maddi  							                                    

 Release date: 12/27/2013
											                                                    
 Description:  This script will create a scheduled tast (InventoryTask) at random time that collects and send inventory to the ServersInventoryDB weekly (every SUNDAY).
		                     

Modifications:                                                                             
                                                                                           
Version	Date Modified		Modified By		Changes Made                                
--------------------------------------------------------------------------------------------
01	1/11/2014		Hamid Maddi		??????		                                                                                            
--------------------------------------------------------------------------------------------
#>

#-------------------------------------------------------------------------------------------

Param ([String] $Arg)

if (!(Test-Path "C:\Inventory\List.txt"))
  {
  "Error, the server list source file List.txt is missing!"
  Break
  }
Function Get-RandomTime
{
    $m = (Get-Random -Minimum 1 -Maximum 1439)*60
    $ts = New-TimeSpan -Seconds $m
    $time='{0:00}:{1:00}:{2:00}' -f $ts.Hours,$ts.Minutes,$ts.Seconds
    return, $time
}

$ServerList = Get-Content "C:\Inventory\List.txt"
#$ErrorActionPreference = "SilentlyContinue"

#SCHTASKS /S  "HND159GCL001" /Run /TN "InventoryTask"

Foreach($server in $ServerList)
{
if (test-connection -computername $server -Quiet)
    {
        $Server
        SCHTASKS /S  $Server /Run /TN "InventoryTask"
        <#
                # Create the inventory folder if it does not exist
                if(!(Test-Path "\\$server\c$\Inventory"))
                { 
                  New-Item -ItemType Directory -Force -Path "\\$server\c$\Inventory"
                }
        
                # Create script to run for the first time
                $ScriptLoaderCode = @'
                # Download loader script from IIS server
                $source = "http://172.31.63.29/ServersInventoryAPI1/resources/ScriptLoader.ps1"
                $destination = "C:\Inventory\ScriptLoader.ps1"
                $wc = New-Object System.Net.WebClient
                $wc.DownloadFile($source, $destination)
     
                # Download inventory script from IIS server
                $source = "http://172.31.63.29/ServersInventoryAPI1/resources/InventoryCollector.ps1"
                $destination = "C:\Inventory\InventoryCollector.ps1"
                $wc = New-Object System.Net.WebClient
                $wc.DownloadFile($source, $destination)

                Start-Sleep -s 5

                if(test-Path C:\Inventory\InventoryCollector.ps1)
                {
                    "C:\Inventory\InventoryCollector.ps1 -ExecutionPolicy Bypass" | Invoke-Expression
                }
        '@

        if(!(Test-Path "\\$server\c$\Inventory\ScriptLoader.ps1")) {Add-Content "\\$server\c$\Inventory\ScriptLoader.ps1" $ScriptLoaderCode}
        `
        # Create Inventory task is it does not exist
        if (!(schtasks /query /S $server /TN "InventoryTask"))
            {
                $time = Get-RandomTime
                $command = "c:\windows\system32\windowspowershell\v1.0\powershell.exe -ExecutionPolicy Bypass -NonInteractive -FILE C:\Inventory\ScriptLoader.ps1" 
                SCHTASKS /Create /S $server /TN "InventoryTask" /TR "$command" /SC WEEKLY /D SUN /ST $time /RL Highest /EC ScriptEvents /RU SYSTEM
            } 
        #>
        }
else { "$server offline "}
}


