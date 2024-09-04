import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { Outlet } from "react-router-dom";
//@ts-ignore
import { loginRequest } from '../authConfig';
import { Button, Container, Navbar, NavDropdown } from "react-bootstrap";


export default function Root() {
    const { instance } = useMsal();
    const activeAccount = instance.getActiveAccount();

    const handleLogoutRedirect = () => {
        instance.logoutPopup().catch((error) => console.log(error));
    };
   
    const handleLoginRedirect = () => {
        instance.loginPopup(loginRequest).catch((error) => console.log(error));
    };

    return (
        <>
             <Navbar className="bg-body-tertiary" sticky="top">
                <Container fluid>
                    <Navbar.Brand href="#home">London Data Service - Re-Identification</Navbar.Brand>
                    <Navbar.Text>
                        <UnauthenticatedTemplate>
                            <div className="collapse navbar-collapse justify-content-end">
                                <Button onClick={handleLoginRedirect}>Sign in</Button>
                            </div>
                        </UnauthenticatedTemplate>
                        <AuthenticatedTemplate>
                            <NavDropdown title={activeAccount?.username} id="collasible-nav-dropdown">
                                <NavDropdown.Item onClick={handleLogoutRedirect}>Sign out</NavDropdown.Item>
                            </NavDropdown>
                        </AuthenticatedTemplate></Navbar.Text>
                </Container>
            </Navbar>

            <Container fluid style={{ position: "absolute", top: "60px", bottom: "0", left: 0, right: 0 }}>
                <AuthenticatedTemplate>
                    <Outlet />
                </AuthenticatedTemplate>
                <UnauthenticatedTemplate>
                    <Container style={{paddingTop:"20px"}}>
                        Please Login
                    </Container>
                </UnauthenticatedTemplate>
            </Container>
        </>
    )
}