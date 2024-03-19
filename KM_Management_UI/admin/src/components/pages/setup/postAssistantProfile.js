import { ref } from 'vue'
// import { GetDetailContentAsync, PatchContentAsync } from '@/api/reqContent.js'
// import { GetCategoryReferenceAsync } from '@/api/reqCategory.js'
import { PostAssistantProfileAsync } from '@/api/reqSetup.js'


const errorInput = ref({
  files: { isError: false, message: '' },
  appName: { isError: false, message: '' },
})


async function HandlePublish(formData) {

  const postProfile = await PostAssistantProfileAsync(formData);

  if (!postProfile.is_success) {
    if (postProfile.error.status === 400 && postProfile.error.errors.Files) {
      errorInput.value.files.isError = true;
      errorInput.value.files.message = postProfile.error.errors.Files;
      
    }else if (postProfile.error.statuscode === 400) {
      errorInput.value.appName.isError = true
      errorInput.value.appName.message = postProfile.error.message
      
    }
    return postProfile.error.statuscode
  }

    errorInput.value.appName.isError = false;
    errorInput.value.files.isError = false;
    errorInput.value.appName.message = "";
    errorInput.value.files.message = "";

  return true;
}


export {
  errorInput,
  HandlePublish,
}