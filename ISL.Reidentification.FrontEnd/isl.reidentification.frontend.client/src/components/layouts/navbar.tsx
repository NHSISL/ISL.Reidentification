import React from 'react';
import { useMsal } from "@azure/msal-react";
import { Button, Container, Navbar } from "react-bootstrap";
import Login from '../login';

interface NavbarComponentProps {
    toggleSidebar: () => void;
    showMenuButton: boolean;
}

const NavbarComponent: React.FC<NavbarComponentProps> = ({ toggleSidebar, showMenuButton }) => {

    return (
        <Navbar className="bg-body-tertiary" sticky="top">
            <Container fluid>
                {showMenuButton && (
                    <Button onClick={toggleSidebar} variant="outline-primary" className="ms-3">
                        <FontAwesomeIcon icon={faCaretUp} className="ps-2" />
                    </Button>
                )}
                <Navbar.Brand href="#home">London Data Service - Re-Identification</Navbar.Brand>
                <Navbar.Text>
                    <Login />
                </Navbar.Text>
            </Container>
        </Navbar>
    );
}

export default NavbarComponent;