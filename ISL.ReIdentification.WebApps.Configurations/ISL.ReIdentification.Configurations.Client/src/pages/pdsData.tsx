import { Container } from "react-bootstrap"
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase"

export const PdsData = () => {
    return (
        <Container fluid className="mt-4">
            <section>
                <BreadCrumbBase
                    link="/home"
                    backLink="Home"
                    currentLink="Pds Data">
                </BreadCrumbBase>
                <div className="mt-3">
                    <h1>Pds Data</h1>
                    <p>Role Needs to be in Security Matrix and Azure AD against user.</p>
                    {/* <UserAccess/>*/}
                </div>
            </section>
        </Container>
    )
}