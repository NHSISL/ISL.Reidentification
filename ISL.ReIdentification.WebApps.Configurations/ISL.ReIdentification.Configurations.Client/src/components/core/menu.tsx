import { faFileLines, faHome, faCog } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState } from 'react';
import { ListGroup } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import { FeatureDefinitions } from '../../featureDefinitions';
import { FeatureSwitch } from '../accessControls/featureSwitch';
import { SecuredComponent } from '../securitys/securedComponents';
import securityPoints from '../../securityMatrix';
import { SecuredLink } from '../securitys/securedLinks';

const MenuComponent: React.FC = () => {
    const location = useLocation();
    const [activePath, setActivePath] = useState(location.pathname);

    const handleItemClick = (path: string) => {
        setActivePath(path);
    };

    return (

        <ListGroup variant="flush" className="text-start border-0">
            <ListGroup.Item
                className={`bg-light ${activePath === '/' ? 'active' : ''}`}
                onClick={() => handleItemClick('/')}>
                <FontAwesomeIcon icon={faHome} className="me-2 fa-icon" />
                <SecuredLink to="/home">Home</SecuredLink>
            </ListGroup.Item>

            <ListGroup.Item
                className={`bg-light ${activePath === '/configuration/home' ? 'active' : ''}`}
                onClick={() => handleItemClick('/configuration/home')}>
                <FontAwesomeIcon icon={faCog} className="me-2 fa-icon" />
                <SecuredLink to="/configuration/home">Configuration</SecuredLink>
            </ListGroup.Item>



            {/*<ListGroup.Item*/}
            {/*    className={`bg-light ${activePath === '/page1/1' ? 'active' : ''}`}*/}
            {/*    onClick={() => handleItemClick('/page1/1')}>*/}
            {/*    <FontAwesomeIcon icon={faFileLines} className="me-2 fa-icon" />*/}
            {/*    <Link to="/page1/1">Page 1 (with url param)</Link>*/}
            {/*</ListGroup.Item>*/}

            {/*<ListGroup.Item*/}
            {/*    className={`bg-light ${activePath === '/page2' ? 'active' : ''}`}*/}
            {/*    onClick={() => handleItemClick('/page2')}>*/}
            {/*    <FontAwesomeIcon icon={faFileLines} className="me-2 fa-icon" />*/}
            {/*    <Link to="/page2">Page 2 (Secured)</Link>*/}
            {/*</ListGroup.Item>*/}

            {/*<FeatureSwitch feature={FeatureDefinitions.Test}>*/}
            {/*    <ListGroup.Item*/}
            {/*        className={`bg-light ${activePath === '/page3' ? 'active' : ''}`}*/}
            {/*        onClick={() => handleItemClick('/page3')}>*/}
            {/*        <FontAwesomeIcon icon={faFileLines} className="me-2 fa-icon" />*/}
            {/*        <Link to="/page3">Page 3 (Feature Switch)</Link>*/}
            {/*    </ListGroup.Item>*/}
            {/*</FeatureSwitch>*/}

            {/*<SecuredComponent allowedRoles={securityPoints.component1.view}>*/}
            {/*    <ListGroup.Item*/}
            {/*        className={`bg-light ${activePath === '/page4' ? 'active' : ''}`}*/}
            {/*        onClick={() => handleItemClick('/page4')}>*/}
            {/*        <FontAwesomeIcon icon={faFileLines} className="me-2 fa-icon" />*/}
            {/*        <Link to="/page4">Page 4 (secureComponent)</Link>*/}
            {/*    </ListGroup.Item>*/}
            {/*</SecuredComponent>*/}

            {/*<ListGroup.Item*/}
            {/*    className={`bg-light ${activePath === '/page5' ? 'active' : ''}`}*/}
            {/*    onClick={() => handleItemClick('/page5')} >*/}
            {/*    <FontAwesomeIcon icon={faFileLines} className="me-2 fa-icon" />*/}
            {/*    <Link to="/page5">Page 5 (no security)</Link>*/}
            {/*</ListGroup.Item>*/}
        </ListGroup>
    );
}

export default MenuComponent;