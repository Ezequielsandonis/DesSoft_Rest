param (
    [string]$ftpServer,
    [string]$ftpUsername,
    [string]$ftpPassword,
    [string]$localPath,
    [string]$remotePath
)

function UploadDirectory {
    param (
        [string]$ftpServer,
        [string]$ftpUsername,
        [string]$ftpPassword,
        [string]$localPath,
        [string]$remotePath
    )

    $localPath = $localPath -replace "\\", "/"
    $remotePath = $remotePath -replace "\\", "/"
    
    Write-Host "Iniciando la carga de archivos desde: $localPath a ftp://$ftpServer/$remotePath"

    foreach ($item in Get-ChildItem -Path $localPath -Recurse) {
        $relativePath = $item.FullName.Substring($localPath.Length).TrimStart("\")
        
        # Asegurarse de que la ruta relativa tenga la barra diagonal correcta
        $ftpUri = "ftp://$ftpServer/$remotePath/$relativePath"
        
        if ($item.PSIsContainer) {
            # Crear directorio en FTP
            $ftpUri = "ftp://$ftpServer/$remotePath/$relativePath/"
            $ftpWebRequest = [System.Net.FtpWebRequest]::Create($ftpUri)
            $ftpWebRequest.Credentials = New-Object System.Net.NetworkCredential($ftpUsername, $ftpPassword)
            $ftpWebRequest.Method = [System.Net.WebRequestMethods+Ftp]::MakeDirectory

            try {
                $ftpWebResponse = $ftpWebRequest.GetResponse()
                $ftpWebResponse.Close()
                Write-Host "Directorio creado exitosamente: $ftpUri"
            } catch {
                Write-Host "Error al crear directorio: $ftpUri - Error: $_"
            }
        } else {
            # Subir archivo a FTP
            $ftpWebRequest = [System.Net.FtpWebRequest]::Create($ftpUri)
            $ftpWebRequest.Credentials = New-Object System.Net.NetworkCredential($ftpUsername, $ftpPassword)
            $ftpWebRequest.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile

            $fileContent = [System.IO.File]::ReadAllBytes($item.FullName)
            if ($fileContent.Length -eq 0) {
                Write-Host "Archivo vacío ignorado: $ftpUri"
                continue
            }

            $ftpWebRequest.ContentLength = $fileContent.Length

            try {
                $requestStream = $ftpWebRequest.GetRequestStream()
                $requestStream.Write($fileContent, 0, $fileContent.Length)
                $requestStream.Close()
                $ftpWebResponse = $ftpWebRequest.GetResponse()
                $ftpWebResponse.Close()
                Write-Host "Archivo subido exitosamente: $ftpUri"
            } catch {
                Write-Host "Error al subir archivo: $ftpUri - Error: $_"
            }
        }
    }
}

# Prueba de conexión FTP
try {
    $ftpTestUri = "ftp://$ftpServer/"
    $ftpTestRequest = [System.Net.FtpWebRequest]::Create($ftpTestUri)
    $ftpTestRequest.Credentials = New-Object System.Net.NetworkCredential($ftpUsername, $ftpPassword)
    $ftpTestRequest.Method = [System.Net.WebRequestMethods+Ftp]::ListDirectory

    $ftpTestResponse = $ftpTestRequest.GetResponse()
    $ftpTestResponse.Close()
    Write-Host "Conexión FTP exitosa."
} catch {
    Write-Host "Error en la conexión FTP: $_"
    exit
}

UploadDirectory -ftpServer $ftpServer -ftpUsername $ftpUsername -ftpPassword $ftpPassword -localPath $localPath -remotePath $remotePath

Write-Host "Despliegue completo"
