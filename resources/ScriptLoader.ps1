<#                                                                                               
 Program Name : ScriptLoader.ps1
                                                                                              
 Original Author: Hamid Maddi  							                                    

 Release date: 12/27/2013
											                                                    
 Description:  This script will first download two scripts (Scriptloader.ps1 and InventoryCollector.ps1), then run the 
	       InventoryCollector.ps1 to collect and send inventory data to the ServersInventory database.
		                     
                                                                                            
Modifications:                                                                             
                                                                                           
Version	Date Modified		Modified By		Changes Made                                
--------------------------------------------------------------------------------------------
01	03/31/2014		Hamid Maddi		Added validation (http port test) before download
--------------------------------------------------------------------------------------------
#>
#---------------- Functions ---------------------#

Function LogEvent ([int]$event_id,$message)
 {

 Try
   {
    switch ($event_id) 
    { 
        0 {$event="FAILURE"} 
        1 {$event="SUCCESS"} 
    }

    $date= Get-Date
    Add-Content   c:\Inventory\log.log "$date    $event    $message"
    }
 Catch
    {
     "an error occured while trying to write to the event log C:\Inventory\Log.log : $_"
    }
 }

Function Test-HTTPport
{
    $test = $false
    $computer = "172.31.63.29"
    $port = 80
    $tcpobject = new-Object system.Net.Sockets.TcpClient 
    #Connect to remote machine's port               
    $connect = $tcpobject.BeginConnect($computer,$port,$null,$null) 
    #Configure a timeout before quitting - time in milliseconds 
    $wait = $connect.AsyncWaitHandle.WaitOne(1000,$false) 
 
    If (-Not $Wait) 
        {
            $test= $false
        } 
    Else 
        {
            $error.clear()
            $tcpobject.EndConnect($connect) | out-Null 
            If ($Error[0]) 
                {
                    LogEvent 0 "an error occurred while trying to connect to 172.31.63.29:80. Error: $error[0].Exception.Message"
                } 
            Else 
                {
                   $test=$true
                }
    }
    return $test
}

#-----------Script Core -----------------

    if(Test-HTTPport -eq $true)
    {
        # Download loader script from IIS server
        $source1 = "http://172.31.63.29/ServersInventoryAPI1/resources/ScriptLoader.ps1"
        $destination1 = "C:\Inventory\ScriptLoader.ps1"
        $wc1 = New-Object System.Net.WebClient
        $wc1.DownloadFile($source1, $destination1)
             
        # Download inventory script from IIS server
        $source2 = "http://172.31.63.29/ServersInventoryAPI1/resources/InventoryCollector.ps1"
        $destination2 = "C:\Inventory\InventoryCollector.ps1"
        $wc2 = New-Object System.Net.WebClient
        $wc2.DownloadFile($source2, $destination2)
        
        LogEvent 1 "ScriptLoader and InventoryCollector were downloaded successfully from IND063GWA029."
        Start-Sleep -s 5
    }
Else
    {
        LogEvent 0 "Could not establish a connection to server 172.31.63.29 at port 80."
    }
   
if(test-Path C:\Inventory\InventoryCollector.ps1)
{
    "C:\Inventory\InventoryCollector.ps1 -ExecutionPolicy Bypass" | Invoke-Expression
}