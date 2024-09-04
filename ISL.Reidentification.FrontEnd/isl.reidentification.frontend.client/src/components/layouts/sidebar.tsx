import React from 'react';
import { Card, ListGroup } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const SideBarComponent: React.FC = () => {

    return (
        <Card className="sidebar" style={{ width: '18rem' }}>
            <Card.Header as="h4">Menu</Card.Header>
            <ListGroup variant="flush">
                <ListGroup.Item>
                    <Link to="/">Home</Link>
                </ListGroup.Item>
                <ListGroup.Item>
                    <Link to="/page1/1">Page 1</Link>
                </ListGroup.Item>
                <ListGroup.Item>
                    <Link to="/page2">Page 2</Link>
                </ListGroup.Item>
            </ListGroup>
        </Card>
    );
}

export default SideBarComponent;