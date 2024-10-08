import { Container } from "react-bootstrap"
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase"

export const DelegatedUserAccess = () => {
    return (
        <Container fluid className="mt-4">
            <section>
                <BreadCrumbBase
                    link="/home"
                    backLink="Home"
                    currentLink="Delegated User Access">
                </BreadCrumbBase>
                <div className="mt-3">
                    <h1>Delegated User Access</h1>
                    <p>Role Needs to be in Security Matrix and Azure AD against user.</p>
                    {/* <UserAccess/>*/}
                </div>
            </section>
        </Container>
    )
}