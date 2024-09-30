import { Container, Row, Col } from "react-bootstrap";

export const Home = () => {
    return (
        <Container fluid className="mt-4">
            <Row className="mb-4">
                <Col>
                    <h1>Welcome to Re-Identification</h1>
                </Col>
            </Row>
            <Row>
                <Col>
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante.
                        Welcome to our site where you can manage various configurations and settings to optimize your experience.
                    </p>
                    <p>
                        Our platform provides a comprehensive suite of tools to help you manage lookups, settings, users, roles,
                        and more. Whether you are an administrator looking to configure system settings or a user looking to
                        customize your experience, we have the tools you need.
                    </p>
                    <p>
                        Explore the various sections of our site to learn more about what we offer and how we can help you
                        achieve your goals. If you have any questions, feel free to reach out to our support team.
                    </p>
                </Col>
            </Row>
        </Container>
    );
}