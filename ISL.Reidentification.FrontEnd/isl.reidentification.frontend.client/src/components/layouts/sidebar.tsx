import { faList, faHome } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React from 'react';
import { Card, ListGroup } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { FeatureDefinitions } from '../../featureDefinitions';
import { FeatureSwitch } from '../accessControls/featureSwitch';
import { SecuredComponent } from '../securitys/securedComponents';
import securityPoints from '../../securityMatrix';

const SideBarComponent: React.FC = () => {

    return (
        <Card className="sidebar border-0 ">
            <Card.Header as="h4">Menu</Card.Header>
            <ListGroup variant="flush" className="text-start border-0" >

                <ListGroup.Item className="bg-light">
                    <FontAwesomeIcon icon={faHome} className="me-2" />
                    <Link to="/">Home</Link>
                </ListGroup.Item>

                <ListGroup.Item className="bg-light">
                    <FontAwesomeIcon icon={faList} className="me-2" />
                    <Link to="/page1/1">Page 1 (Not Secure)</Link>
                </ListGroup.Item>

                <ListGroup.Item className="bg-light">
                    <FontAwesomeIcon icon={faList} className="me-2" />
                    <Link to="/page2">Page 2 (Secured)</Link>
                </ListGroup.Item>

                <FeatureSwitch feature={FeatureDefinitions.Test}>
                    <ListGroup.Item className="bg-light">
                        <FontAwesomeIcon icon={faList} className="me-2" />
                        <Link to="/page3">Page 3 (Feature Switch)</Link>
                    </ListGroup.Item>
                </FeatureSwitch>

                <SecuredComponent allowedRoles={securityPoints.component1.view}>
                    <ListGroup.Item className="bg-light">
                        <FontAwesomeIcon icon={faList} className="me-2" />
                        <Link to="/page4">Page 4 (secureComponent)</Link>
                    </ListGroup.Item>
                </SecuredComponent>

            </ListGroup>
        </Card>
    );
}

export default SideBarComponent;