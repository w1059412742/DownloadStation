import axios from 'axios';

async function testDelete() {
    try {
        console.log("Logging in...");
        const loginRes = await axios.post("http://localhost:5186/api/admin/auth/login", {
            username: "admin",
            password: "password"
        });
        const token = loginRes.data.data.token;
        console.log("Got token.");

        console.log("Fetching softwares...");
        const getRes = await axios.get("http://localhost:5186/api/admin/softwares", {
            headers: { Authorization: `Bearer ${token}` }
        });

        const softwares = getRes.data.data.items;
        if (softwares.length === 0) {
            console.log("No softwares to delete.");
            return;
        }

        const softwareId = softwares[0].id;
        console.log("Deleting software " + softwareId + "...");

        try {
            const deleteRes = await axios.delete("http://localhost:5186/api/admin/softwares/" + softwareId, {
                headers: { Authorization: `Bearer ${token}` }
            });
            console.log("Delete response:", deleteRes.data);
        } catch (e) {
            console.error("Delete software failed:", e.response ? e.response.data : e.message);
        }

    } catch (e) {
        console.error("Error:", e.response ? e.response.data : e.message);
    }
}

testDelete();
