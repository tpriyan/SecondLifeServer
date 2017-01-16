<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" UnobtrusiveValidationMode="None" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Texture changer</title>
   
    <style type="text/css">
        #Form1 {}
        .auto-style1 {
            text-align: center;
        }
    </style>
   
</head>
<body>
  <form runat="server" id ="Form1">

     <div align="center">
         <h1> Login to rentals</h1>

     </div>

       <div align="center">
      <asp:Login ID = "Login1" runat = "server" OnAuthenticate= "ValidateUser" ></asp:Login>
</div>
  </form>
   

   
  
</body>
</html>
