# claramontana-online-shop

Web API application. </br>
Every method related to products (and the logout method as well) is authenticated using JSON Web Tokens.</br>
Built in ASP.NET 5.0 and C#. </br>
Database Management System: SQL Server 2019.


<img width="943" alt="claramontana1" src="https://user-images.githubusercontent.com/50749737/171925223-54ba420f-bfcc-450a-afa9-b7a8fcc3b88d.png">

<img width="942" alt="claramontana2" src="https://user-images.githubusercontent.com/50749737/171926046-be605475-a1c1-4755-8ce6-43af4018ec47.png">

<h2>Getting Started</h2>

    Download the project
    Open project with Visual Studio and wait for dependencies to be resolved
    Set ClaramontanaOnlineShop.WebApi as Startup project
    Open Package Manager Console and make sure that Default project is ClaramontanaOnlineShop.Data
    Run the following command: update-database
    If you need sample database:
        - Go to https://github.com/KarimMessaoud/Demo-Data-Scripts/blob/main/Claramontana_DemoData.sql
        - Copy the contents of Claramontana_DemoData.sql file
        - Open SQL Server Management Studio and connect to server
        - Click New Query on the top bar, paste data which was copied and click Execute (below the New Query button)

<h6>After running the application always follow the Swagger documentation which will appear on the screen and take into account the following steps:</h6>
	<ul>
		<li> Firstly, if you want to create your own user, you have to send the register request</li>
                <li>If not, use the sample user with the username: john and the password: Demo123$</li>
		<li>Then click on the Authorize button on the right side of the screen and enter the access token which you have received as a response in the login request</li>
<li>Then you can test all the other methods related to products</li>
	</ul>
