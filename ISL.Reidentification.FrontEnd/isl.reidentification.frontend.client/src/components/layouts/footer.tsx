import React from 'react';
import { Col, Container, Row } from "react-bootstrap";

const FooterComponent: React.FC = () => {
    return (
        <Container fluid style={{ height:'50px', backgroundColor: '#f8f9fa', padding: '10px 0', position: 'absolute', bottom: 0, width: '100%' }}>
            <Row>
                <Col style={{ textAlign: "left" }} className="ms-4">
                    <p>2024 North East London ICB. All rights reserved.</p>
                </Col>
            </Row>
        </Container>
    );
}

export default FooterComponent;