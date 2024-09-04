import { Container } from "react-bootstrap";
import { useParams } from "react-router-dom";

export const Page1 = () => {
    let { id } = useParams();

    return (
        <Container>
            <h1>Page 1</h1>
            <p>Page 1 content - {id}</p>
        </Container>
    )
}