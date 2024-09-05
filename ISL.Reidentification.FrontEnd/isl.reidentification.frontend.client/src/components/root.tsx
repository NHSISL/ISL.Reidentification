import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import { Outlet } from "react-router-dom";
import NavbarComponent from "./layouts/navbar";
import { Col, Container, Row } from "react-bootstrap";
import FooterComponent from "./layouts/footer";
import { useState } from "react";
import SideBarComponent from "./layouts/sidebar";

export default function Root() {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    const toggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
    };

    return (
        <>
            <Container fluid className="d-flex flex-column min-vh-100 p-0">
                <Row className="flex-grow-1">

                    <Col xs={2} style={{ width: '300px' }} className={`d-flex flex-column bg-light p-0 ${sidebarOpen ? 'sidebar-open' : 'sidebar-closed'}`}>
                        <AuthenticatedTemplate>
                            <SideBarComponent />
                        </AuthenticatedTemplate>
                    </Col>

                    <Col className={`d-flex flex-column p-0 ${sidebarOpen ? 'content-shift-right' : 'content-shift-left'}`}>
                        <NavbarComponent toggleSidebar={toggleSidebar} showMenuButton={true} />
                        <Container fluid className="d-flex flex-column flex-grow-1 p-0">
                            <Row className="flex-grow-1">
                                <Col className="d-flex flex-column">
                                    <AuthenticatedTemplate>
                                        <Outlet />
                                    </AuthenticatedTemplate>
                                    <UnauthenticatedTemplate>
                                        <Container className="d-flex justify-content-center align-items-center flex-grow-1" style={{ marginTop: '-20%' }}>
                                            <h2>Please Login</h2>
                                        </Container>
                                    </UnauthenticatedTemplate>
                                </Col>
                            </Row>
                            <FooterComponent />
                        </Container>
                    </Col>

                </Row>
            </Container>
        </>
    )
}