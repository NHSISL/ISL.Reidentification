import React from 'react';
import { useMsal } from "@azure/msal-react";
import { Container, Navbar } from "react-bootstrap";
import Login from '../login';

const NavbarComponent: React.FC = () => {
    return (
        <Navbar className="bg-body-tertiary" sticky="top">
            <Container fluid>
                <Navbar.Brand href="#home">London Data Service - Re-Identification</Navbar.Brand>
                <Navbar.Text>
                    <Login />
                </Navbar.Text>
            </Container>
        </Navbar>
    );
}

export default NavbarComponent;