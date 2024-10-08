import { Container } from "react-bootstrap"
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase"

export const OdsData = () => {
    return (
        <Container fluid className="mt-4">
            <section>
                <BreadCrumbBase
                    link="/home"
                    backLink="Home"
                    currentLink="ODS Data">
                </BreadCrumbBase>
                <div className="mt-3">
                    <h1>Ods Data</h1>
                    <p>Role Needs to be in Security Matrix and Azure AD against user.</p>
                </div>
            </section>
        </Container>
    )
}