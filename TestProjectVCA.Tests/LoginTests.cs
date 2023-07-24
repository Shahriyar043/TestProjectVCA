using System;
using System.Web.UI.WebControls;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestProjectVCA;
using Microsoft.AspNet.Identity;

[TestClass]
public class LoginTests
{
    [TestMethod]
    public void LogIn_ValidCredentials_RedirectsToReturnUrl()
    {
        // Arrange
        var loginPage = new TestProjectVCA.Account.Login();
        var mockManager = new Mock<UserManager>();
        var mockUser = new Mock<ApplicationUser>();
        var mockRequest = new Mock<HttpRequestBase>();
        var mockResponse = new Mock<HttpResponseBase>();

        mockUser.Setup(u => u.UserName).Returns("shahriyar");
        mockManager.Setup(m => m.Find("shahriyar", "lerik043")).Returns(mockUser.Object);

        mockRequest.SetupGet(x => x.QueryString).Returns(new System.Collections.Specialized.NameValueCollection() { { "ReturnUrl", "someurl" } });
        var mockHttpContext = new Mock<HttpContextBase>();
        mockHttpContext.SetupGet(x => x.Request).Returns(mockRequest.Object);

        mockResponse.Setup(x => x.Redirect(It.IsAny<string>())).Verifiable();

        IdentityHelper.SignIn(mockManager.Object, mockUser.Object, true);

        // Act
        loginPage.LogIn(null, EventArgs.Empty);

        // Assert
        mockResponse.Verify(); // Verify that Response.Redirect was called
    }

    [TestMethod]
    public void LogIn_InvalidCredentials_ShowsErrorMessage()
    {
        // Arrange
        var loginPage = new TestProjectVCA.Account.Login();
        var mockManager = new Mock<UserManager>();
        var mockRequest = new Mock<HttpRequestBase>();
        var mockResponse = new Mock<HttpResponseBase>();

        mockManager.Setup(m => m.Find("shahriyar1", "lerik043545")).Returns((ApplicationUser)null);

        mockRequest.SetupGet(x => x.QueryString).Returns(new System.Collections.Specialized.NameValueCollection());
        var mockHttpContext = new Mock<HttpContextBase>();
        mockHttpContext.SetupGet(x => x.Request).Returns(mockRequest.Object);

        loginPage.UserName = new TextBox { Text = "shahriyar1" };
        loginPage.Password = new TextBox { Text = "lerik043545" };
        loginPage.RememberMe = new CheckBox { Checked = true };

        loginPage.FailureText = new Literal();
        loginPage.ErrorMessage = new PlaceHolder();
        loginPage.FailureText.Text = "";

        // Act
        loginPage.LogIn(null, EventArgs.Empty);

        // Assert
        Assert.AreEqual("Invalid username or password.", loginPage.FailureText.Text);
        Assert.IsTrue(loginPage.ErrorMessage.Visible);
        IdentityHelper.SignIn(mockManager.Object, It.IsAny<ApplicationUser>(), It.IsAny<bool>());

        mockResponse.Verify(x => x.Redirect(It.IsAny<string>()), Times.Never); 
    }
}
