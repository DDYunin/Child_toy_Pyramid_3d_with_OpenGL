#version 330 core
struct Material {
    sampler2D diffuse; //Диффузная часть модели фонга. // Это та часть света, которая дает больше всего, это цвет объекта, на который попадает свет.
    sampler2D specular; // Зеркальный свет - это свет, исходящий от объекта, подобно свету, падающему на металл.
    float     shininess; //Блеск - это сила, до которой доводится зеркальный свет
};

//// Прожектор по сути является точечным источником света, однако мы хотим показывать свет только под определенным углом.
////Этот угол является cutoff, the outercutoff используется для создания более плавной границы для прожектора.
//Источник света содержит все значения от источника света, как рассеянные, так и зеркальные значения от источника света.

struct Light {
    vec3  position;
    vec3  direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main()
{
    //окружающая среда
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    //рассеянный
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));

    //зеркальный
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

    //затухание
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    //интенсивность прожектора
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); //Интенсивность - это интенсивность света на данном фрагменте,
                                                                              // используется для создания плавной границы.  
    //При применении интенсивности прожектора мы хотим ее умножить.
    ambient  *= attenuation; //// Помните, что окружающая среда - это то место, куда не попадает свет, это означает, что прожектор не должен быть применен
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
    
}