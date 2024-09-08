$(document).ready(function () {
    // Initialisation du DataTable lorsque le document est prêt
    var table = $('#userTable').DataTable({
        pageLength: 10, // Nombre de lignes affichées par page
        lengthMenu: [10, 25, 50, 75, 100], // Options de sélection du nombre de lignes par page
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.5/i18n/fr-FR.json" // URL de la traduction en français pour les éléments de DataTable
        },
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'], // Boutons pour exporter les données
        initComplete: function () {
            var api = this.api();

            // Appliquer une recherche par colonne en utilisant les champs de recherche dans le pied de page
            api.columns().every(function (index) {
                if (index < api.columns().count() - 1) { // Exclure la colonne des actions pour la recherche
                    var column = this;
                    // Ajouter un événement 'keyup' et 'change' sur chaque champ de recherche
                    $('input', $(api.column(index).footer())).on('keyup change', function () {
                        if (column.search() !== this.value) {
                            column.search(this.value).draw(); // Appliquer la recherche et redessiner le tableau
                        }
                    });
                }
            });
        }
    });
});

// Fonction pour supprimer un utilisateur
function deleteUser(userId, rowElement) {
    fetch(AdminDeleteFleetUserUrl, { // URL pour supprimer l'utilisateur
        method: 'POST', // Méthode de la requête
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded' // Type de contenu pour la requête
        },
        body: new URLSearchParams({
            'fleetUserId' : userId // Paramètre pour identifier l'utilisateur à supprimer
        })
    })
        .then(response => {
            if (response.ok) {
                return response.text(); // Traiter la réponse si elle est correcte
            } else {
                return response.text().then(text => { throw new Error(text); }); // Lancer une erreur si la réponse n'est pas correcte
            }
        })
        .then(result => {
            Swal.fire(
                'Supprimé !', // Titre de l'alerte de succès
                'L\'utilisateur a été supprimé.', // Texte de l'alerte de succès
                'success' // Type de l'alerte de succès
            ).then(() => {
                // Supprimer la ligne du tableau si l'élément de ligne est fourni
                if (rowElement) {
                    rowElement.remove();
                    
                }
            });
        })
        .catch(error => {
            Swal.fire(
                'Erreur !', // Titre de l'alerte d'erreur
                'Une erreur est survenue lors de la suppression de l\'utilisateur : ' + error.message, // Texte de l'alerte d'erreur
                'error' // Type de l'alerte d'erreur
            );
        });
}

// Fonction pour confirmer la suppression d'un utilisateur
function confirmDelete(userId, buttonElement) {
    Swal.fire({
        title: 'Êtes-vous sûr ?', // Titre de la fenêtre de confirmation
        text: "Vous ne pourrez pas récupérer cet utilisateur !", // Message d'avertissement
        icon: 'warning', // Type d'alerte (avertissement)
        showCancelButton: true, // Afficher le bouton d'annulation
        confirmButtonColor: '#3085d6', // Couleur du bouton de confirmation
        cancelButtonColor: '#d33', // Couleur du bouton d'annulation
        confirmButtonText: 'Oui, supprimer !', // Texte du bouton de confirmation
        cancelButtonText: 'Annuler' // Texte du bouton d'annulation
    }).then((result) => {
        if (result.isConfirmed) {
            // Si l'utilisateur confirme, passer l'élément de ligne à la fonction de suppression
            var rowElement = buttonElement.closest('tr');
            deleteUser(userId, rowElement);
        }
    });
}

function showEditPopup(userId, buttonElement) {
    $.ajax({
        url: AdminEditUserUrl,
        type: 'GET',
        data: { fleetUserId: userId },
        success: function (data) {
            Swal.fire({
                title: 'Modifier l\'utilisateur',
                html: data,
                width: '80%',
                showConfirmButton: false,
                showCloseButton: true,
                didOpen: () => {
                    // Appliquer le style pour rendre le popup déplacable et redimensionnable
                    const popupElement = Swal.getPopup();
                    $(popupElement).draggable({
                        handle: '.swal2-title' // Utiliser l'en-tête comme zone de déplacement
                    }).resizable({
                        // Options de redimensionnement
                        handles: 'n, e, s, w, ne, se, sw, nw', // Zones de redimensionnement
                        minWidth: 300, // Largeur minimale
                        minHeight: 200, // Hauteur minimale
                        alsoResize: popupElement.querySelector('.swal2-content') // Redimensionner le contenu en même temps
                    });

                    // Gérer la soumission du formulaire dans le popup
                    $('#editUserForm').on('submit', function (e) {
                        e.preventDefault(); // Empêcher le rechargement de la page
                        submitEditForm(this, buttonElement);
                    });
                }
            });
        },
        error: function () {
            Swal.fire('Erreur !', 'Impossible de charger le formulaire de modification.', 'error');
        }
    });
}

function submitEditForm(form, buttonElement) {
    fetch(AdminUpdateUserUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams(new FormData(form))
    })
        .then(response => response.json())
        .then(data => {
            if (data.errors) {
                // Effacer les erreurs précédentes
                document.querySelectorAll('.error').forEach(el => el.remove());

                // Afficher les nouvelles erreurs sous les champs correspondants
                for (const [field, message] of Object.entries(data.errors)) {
                    const fieldElement = form.querySelector(`[name="${field}"]`);
                    if (fieldElement) {
                        const errorElement = document.createElement('div');
                        errorElement.className = 'error';
                        errorElement.textContent = message;
                        fieldElement.parentElement.appendChild(errorElement);
                    }
                }
                return;
            }

            Swal.fire(
                'Modifié !',
                'L\'utilisateur a été modifié.',
                'success'
            ).then(() => {
                Swal.close(); // Fermer la popup après la modification réussie

                const row = document.getElementById('row-' + data.id);
                row.querySelector('.userNameCell').textContent = data.name;
                row.querySelector('.userSurNameCell').textContent = data.surname;
                row.querySelector('.userMatriculeCell').textContent = data.matricule;
                row.querySelector('.userCNICell').textContent = data.cniNumber;
                row.querySelector('.userEmailCell').textContent = data.email;
                row.querySelector('.userStatusCell').textContent = data.status == 1 ? "Actif" : "Inactif";
            });
        })
        .catch(error => {
            Swal.fire('Erreur !', 'Une erreur est survenue : ' + error.message, 'error');
        });
}
