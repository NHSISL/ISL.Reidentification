import { Card, CardBody, CardText, CardTitle, Container } from "react-bootstrap";
import { Link } from "react-router-dom";

export const ConfigurationHome = () => {

    return (
        <Container fluid>
            <h1 className="display-5 fw-bold">Configuration</h1>

            <Card>
                <CardBody>
                    <CardTitle>
                        <Link to={'/configuration/lookups'}>
                            Lookups
                        </Link>
                    </CardTitle>
                    <CardText>
                        View, add, edit and remove lookups.
                    </CardText>
                </CardBody>
            </Card>
        </Container>
    )
}