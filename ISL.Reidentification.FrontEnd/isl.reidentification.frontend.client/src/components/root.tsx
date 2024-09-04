import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import { Outlet } from "react-router-dom";
import NavbarComponent from "./layouts/navbar";
import { Col, Container, Row } from "react-bootstrap";
import FooterComponent from "./layouts/footer";

export default function Root() {

    return (
        <>
            <NavbarComponent />
            <Container fluid className="d-flex flex-column min-vh-100 p-0">
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
        </>
    )
}