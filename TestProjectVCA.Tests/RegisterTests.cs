using System;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestProjectVCA;

[TestClass]
public class RegisterTests
{
    [TestMethod]
    public void CreateUser_Click_ValidUser_RedirectsToReturnUrl()
    {
        // Arrange
        var resgiterPage = new TestProjectVCA.Account.Register();
        var mockManager = new Mock<UserManager>();
        var mockUser = new Mock<ApplicationUser>();
        var mockResponse = new Mock<HttpResponseBase>();

        mockManager.Setup(m => m.Create(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(IdentityResult.Success);

        // Set up the UserName and Password for a new user
        resgiterPage.UserName = new TextBox { Text = "testuser" };
        resgiterPage.Password = new TextBox { Text = "password123" };

        mockResponse.Setup(x => x.Redirect(It.IsAny<string>())).Verifiable();
        IdentityHelper.SignIn(mockManager.Object, mockUser.Object, false);

        // Act
        resgiterPage.CreateUser_Click(null, EventArgs.Empty);

        // Assert
        mockResponse.Verify();
    }

    [TestMethod]
    public void CreateUser_Click_InvalidUser_ShowsErrorMessage()
    {
        // Arrange
        var registerPage = new TestProjectVCA.Account.Register();
        var mockManager = new Mock<UserManager>();
        var mockResponse = new Mock<HttpResponseBase>();

        var errorMessages = new[] { "Password must be at least 6 characters long." };
        mockManager.Setup(m => m.Create(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(IdentityResult.Failed(errorMessages));

        registerPage.UserName = new TextBox { Text = "testuser" };
        registerPage.Password = new TextBox { Text = "pwd" };

        // Set up the ErrorMessage for verification
        registerPage.ErrorMessage = new Literal();
        registerPage.ErrorMessage.Text = ""; 

        // Act
        registerPage.CreateUser_Click(null, EventArgs.Empty);

        // Assert
        Assert.AreEqual("Password must be at least 6 characters long.", registerPage.ErrorMessage.Text);
        IdentityHelper.SignIn(mockManager.Object, It.IsAny<ApplicationUser>(), It.IsAny<bool>());
        mockResponse.Verify(x => x.Redirect(It.IsAny<string>()), Times.Never); 
    }
}
