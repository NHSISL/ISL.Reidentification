import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars} from '@fortawesome/free-solid-svg-icons';
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
                        <FontAwesomeIcon icon={faBars} />
                    </Button>
                )}
                <Navbar.Brand href="/" className="me-auto ms-3">London Data Service - Re-Identification</Navbar.Brand>
                <Navbar.Text>
                    <Login />
                </Navbar.Text>
            </Container>
        </Navbar>
    );
}

export default NavbarComponent;