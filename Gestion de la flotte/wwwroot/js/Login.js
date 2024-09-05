document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("login-form");
    const submitButton = document.getElementById("submit-button");
    const submitText = document.getElementById("submit-text");
   
    form.addEventListener("submit", async (event) => {

    
    
        // Désactiver le bouton pour éviter plusieurs soumissions
        submitButton.disabled = true;
        // Changer le texte du bouton en "Connexion..."
        submitText.textContent = 'Connexion...';
    });
});



    // Gestion de la visibilité du mot de passe
    document.querySelectorAll('.toggle-password').forEach(icon => {
        icon.addEventListener('click', function () {
            const targetInput = document.querySelector(this.getAttribute('data-target'));
            if (!targetInput) return;

            const isPasswordVisible = targetInput.type === 'text';
            targetInput.type = isPasswordVisible ? 'password' : 'text';
            this.classList.toggle('fa-eye', isPasswordVisible);
            this.classList.toggle('fa-eye-slash', !isPasswordVisible);
        });
    });

