

Imports System
Imports System.IO
Imports System.Security.Cryptography



Class AesCryp

    Public Shared Function Encrypt(ByVal plainText As String, ByVal Key As String, ByVal IV As String) As Byte()

        Dim convertedKey() As Byte = System.Text.Encoding.ASCII.GetBytes(Key.Replicate(8).TakeLeft(32))
        Dim convertedIV() As Byte = System.Text.Encoding.ASCII.GetBytes(IV.Replicate(4).TakeLeft(16))

        Return EncryptStringToBytes_Aes(plainText, convertedKey, convertedIV)

    End Function

    Public Shared Function Decrypt(ByVal cipherText() As Byte, ByVal Key As String, ByVal IV As String) As String

        Dim convertedKey() As Byte = System.Text.Encoding.ASCII.GetBytes(Key.Replicate(8).TakeLeft(32))
        Dim convertedIV() As Byte = System.Text.Encoding.ASCII.GetBytes(IV.Replicate(4).TakeLeft(16))

        Return DecryptStringFromBytes_Aes(cipherText, convertedKey, convertedIV)

    End Function

    

    Private Shared Shadows Function EncryptStringToBytes_Aes(ByVal plainText As String, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()
        ' Check arguments.
        If plainText Is Nothing OrElse plainText.Length <= 0 Then
            Throw New ArgumentNullException("plainText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        Dim encrypted() As Byte
        ' Create an Aes object
        ' with the specified key and IV.
        Using aesAlg As Aes = Aes.Create()

            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
            ' Create the streams used for encryption.
            Using msEncrypt As New MemoryStream()
                Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                    Using swEncrypt As New StreamWriter(csEncrypt)

                        'Write all data to the stream.
                        swEncrypt.Write(plainText)
                    End Using
                    encrypted = msEncrypt.ToArray()
                End Using
            End Using
        End Using

        ' Return the encrypted bytes from the memory stream.
        Return encrypted

    End Function 'EncryptStringToBytes_Aes

    Private Shared Shadows Function DecryptStringFromBytes_Aes(ByVal cipherText() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As String
        ' Check arguments.
        If cipherText Is Nothing OrElse cipherText.Length <= 0 Then
            Throw New ArgumentNullException("cipherText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        ' Declare the string used to hold
        ' the decrypted text.
        Dim plaintext As String = Nothing

        ' Create an Aes object
        ' with the specified key and IV.
        Using aesAlg As Aes = Aes.Create()
            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for decryption.
            Using msDecrypt As New MemoryStream(cipherText)

                Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

                    Using srDecrypt As New StreamReader(csDecrypt)


                        ' Read the decrypted bytes from the decrypting stream
                        ' and place them in a string.
                        plaintext = srDecrypt.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using

        Return plaintext

    End Function 'DecryptStringFromBytes_Aes 

End Class 'AesExample